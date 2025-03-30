<template>
  <div class="segment-container">
    <div v-for="segment in segments" :key="segment.SegmentId" class="segment">
      <div class="segment-header">
        {{ segment.SegmentId }}
        <span class="segment-help" @click="showSegmentHelp(segment.SegmentId)">?</span>
      </div>
      <div class="segment-fields">
        <div v-for="(field, index) in segment.Fields" :key="index" 
             class="field" @mouseover="handleHover(segment, index, $event)">
          <span class="field-index">{{ index + 1 }}:</span>
          <span class="field-value">{{ field || '""' }}</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    segments: {
      type: Array,
      required: true
    }
  },
  methods: {
    handleHover(segment, fieldIndex, event) {
      this.$emit('hover', {
        segmentId: segment.SegmentId,
        fieldIndex: fieldIndex + 1,
        position: {
          x: event.clientX,
          y: event.clientY
        }
      });
    },
    showSegmentHelp(segmentId) {
      const helpTexts = {
        MSH: "Message Header - Contains message metadata",
        PID: "Patient Identification - Contains patient demographics",
        PV1: "Patient Visit - Contains visit information",
        NK1: "Next of Kin - Contains emergency contact information",
        OBX: "Observation/Result - Contains clinical observations",
        NTE: "Notes and Comments - Contains free-text notes"
      };
      alert(helpTexts[segmentId] || `No help available for ${segmentId} segment`);
    }
  }
}
</script>

<style scoped>
.segment-container {
  font-family: 'Courier New', monospace;
}

.segment {
  margin-bottom: 1.5rem;
  border: 1px solid #e0e0e0;
  border-radius: 4px;
  overflow: hidden;
}

.segment-header {
  background-color: #f5f5f5;
  padding: 0.5rem 1rem;
  font-weight: bold;
  display: flex;
  justify-content: space-between;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.segment-help {
  cursor: help;
  color: #666;
  font-weight: normal;
}

.segment-fields {
  padding: 0.5rem 1rem;
}

.field {
  display: flex;
  margin-bottom: 0.25rem;
  line-height: 1.4;
}

.field:hover {
  background-color: #f9f9f9;
}

.field-index {
  color: #666;
  min-width: 2.5rem;
  margin-right: 0.5rem;
  text-align: right;
}

.field-value {
  white-space: pre-wrap;
  word-break: break-all;
}
</style>