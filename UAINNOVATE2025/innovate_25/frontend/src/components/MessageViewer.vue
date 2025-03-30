<template>
  <div class="message-viewer">
    <div class="controls">
      <button @click="processFile" :disabled="processing" class="process-btn">
        {{ processing ? 'Processing...' : 'Process HL7 File' }}
      </button>
      
      <div class="filter-controls">
        <select v-model="filterField" class="filter-select">
          <option value="mrn">MRN</option>
          <option value="name">Patient Name</option>
          <option value="controlId">Message ID</option>
        </select>
        <input v-model="searchTerm" placeholder="Search..." class="search-input">
      </div>
      
      <div class="view-options">
        <button @click="toggleViewMode" class="view-toggle">
          {{ viewMode === 'single' ? 'Compare Views' : 'Single View' }}
        </button>
        <div class="download-group">
          <button @click="downloadFile('messages_sorted.txt')" class="download-btn">
            Sorted
          </button>
          <button @click="downloadFile('messages_redacted.txt')" class="download-btn">
            Redacted
          </button>
          <button @click="downloadFile('messages_deidentified.txt')" class="download-btn">
            De-identified
          </button>
        </div>
      </div>
    </div>

    <div v-if="loading" class="loading-indicator">
      <div class="spinner"></div>
      Loading messages...
    </div>

    <div v-else-if="currentMessage" class="message-display">
      <SideBySideView 
        v-if="viewMode === 'compare'"
        :original-segments="currentMessage.original.Segments"
        :deidentified-segments="currentMessage.deidentified.Segments"
        @hover="showTooltip"
      />
      
      <div v-else class="single-message-view">
        <HL7SegmentDisplay 
          :segments="currentViewSegments"
          @hover="showTooltip"
        />
      </div>

      <div class="pagination">
        <button @click="prevPage" :disabled="currentPage === 1" class="page-btn">
          Previous
        </button>
        <span class="page-info">Page {{ currentPage }} of {{ totalPages }}</span>
        <button @click="nextPage" :disabled="currentPage === totalPages" class="page-btn">
          Next
        </button>
      </div>
    </div>

    <div v-if="tooltip.visible" class="hl7-tooltip" :style="tooltipStyle">
      {{ tooltip.content }}
    </div>
  </div>
</template>

<script>
import HL7SegmentDisplay from './HL7SegmentDisplay.vue'
import SideBySideView from './SideBySideView.vue'

export default {
  components: { HL7SegmentDisplay, SideBySideView },
  data() {
    return {
      messages: [],
      processing: false,
      loading: false,
      viewMode: 'single', // 'single' or 'compare'
      filterField: 'mrn',
      searchTerm: '',
      currentPage: 1,
      pageSize: 5,
      tooltip: {
        visible: false,
        content: '',
        x: 0,
        y: 0
      }
    }
  },
  computed: {
    filteredMessages() {
      if (!this.searchTerm.trim()) return this.messages;
      
      const term = this.searchTerm.toLowerCase();
      return this.messages.filter(msg => {
        const value = this.filterField === 'mrn' ? msg.original.GetField("PID-3.1") :
                     this.filterField === 'name' ? msg.original.GetField("PID-5.1") :
                     msg.original.MessageControlId;
        return value?.toLowerCase().includes(term);
      });
    },
    paginatedMessages() {
      const start = (this.currentPage - 1) * this.pageSize;
      return this.filteredMessages.slice(start, start + this.pageSize);
    },
    totalPages() {
      return Math.ceil(this.filteredMessages.length / this.pageSize);
    },
    currentMessage() {
      return this.paginatedMessages[0] || null;
    },
    currentViewSegments() {
      if (!this.currentMessage) return [];
      return this.viewMode === 'single' 
        ? this.currentMessage.deidentified.Segments
        : [];
    },
    tooltipStyle() {
      return {
        left: `${this.tooltip.x + 10}px`,
        top: `${this.tooltip.y + 10}px`
      };
    }
  },
  methods: {
    async processFile() {
      this.processing = true;
      try {
        const response = await this.$http.post('/api/hl7/process');
        this.loadMessages();
      } catch (error) {
        console.error('Processing failed:', error);
        alert('Failed to process HL7 file');
      } finally {
        this.processing = false;
      }
    },
    async loadMessages() {
      this.loading = true;
      try {
        const response = await this.$http.get('/api/hl7/download/messages_deidentified.txt');
        // In a real app, you would parse the response and update the messages
        // This is simplified for demonstration
        this.messages = []; // Would be populated with actual parsed messages
      } catch (error) {
        console.error('Failed to load messages:', error);
      } finally {
        this.loading = false;
      }
    },
    toggleViewMode() {
      this.viewMode = this.viewMode === 'single' ? 'compare' : 'single';
    },
    showTooltip(event) {
      this.tooltip = {
        visible: true,
        content: `${event.segmentId}-${event.fieldIndex}`,
        x: event.position.x,
        y: event.position.y
      };
      setTimeout(() => this.tooltip.visible = false, 2000);
    },
    async downloadFile(filename) {
      try {
        const response = await this.$http.get(`/api/hl7/download/${filename}`, {
          responseType: 'blob'
        });
        
        const url = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', filename);
        document.body.appendChild(link);
        link.click();
        link.remove();
      } catch (error) {
        console.error('Download failed:', error);
        alert('Failed to download file');
      }
    },
    nextPage() {
      if (this.currentPage < this.totalPages) this.currentPage++;
    },
    prevPage() {
      if (this.currentPage > 1) this.currentPage--;
    }
  },
  mounted() {
    this.loadMessages();
  }
}
</script>

<style scoped>
.message-viewer {
  background: white;
  border-radius: 8px;
  padding: 1.5rem;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.controls {
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
  margin-bottom: 1.5rem;
  align-items: center;
}

.process-btn {
  background: #005b94;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  cursor: pointer;
  font-weight: bold;
}

.filter-controls {
  display: flex;
  gap: 0.5rem;
  flex-grow: 1;
}

.filter-select {
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  min-width: 120px;
}

.search-input {
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  flex-grow: 1;
  min-width: 200px;
}

.view-options {
  display: flex;
  gap: 0.5rem;
}

.view-toggle {
  background: #4a90e2;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  cursor: pointer;
}

.download-group {
  display: flex;
  gap: 0.25rem;
}

.download-btn {
  background: #50b83c;
  color: white;
  border: none;
  padding: 0.5rem;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.8rem;
}

.loading-indicator {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  padding: 2rem;
  color: #666;
}

.spinner {
  border: 3px solid #f3f3f3;
  border-top: 3px solid #005b94;
  border-radius: 50%;
  width: 20px;
  height: 20px;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.message-display {
  margin-top: 1rem;
}

.single-message-view {
  background: #f8f9fa;
  padding: 1rem;
  border-radius: 4px;
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 1rem;
  margin-top: 1.5rem;
}

.page-btn {
  background: #f0f0f0;
  border: 1px solid #ddd;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  cursor: pointer;
}

.page-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.page-info {
  color: #666;
}

.hl7-tooltip {
  position: fixed;
  background: rgba(0, 0, 0, 0.8);
  color: white;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  pointer-events: none;
  z-index: 100;
  font-size: 0.9rem;
}
</style>