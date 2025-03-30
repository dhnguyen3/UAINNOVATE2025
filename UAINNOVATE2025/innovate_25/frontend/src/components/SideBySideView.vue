<template>
  <div class="comparison-container">
    <div class="message-panel original">
      <h3>Original Message</h3>
      <HL7SegmentDisplay 
        :segments="originalSegments" 
        @hover="emitHover"
      />
    </div>
    <div class="message-panel deidentified">
      <h3>De-identified Message</h3>
      <HL7SegmentDisplay 
        :segments="deidentifiedSegments" 
        @hover="emitHover"
      />
    </div>
  </div>
</template>

<script>
import HL7SegmentDisplay from './HL7SegmentDisplay.vue'

export default {
  components: { HL7SegmentDisplay },
  props: {
    originalSegments: {
      type: Array,
      required: true
    },
    deidentifiedSegments: {
      type: Array,
      required: true
    }
  },
  methods: {
    emitHover(event) {
      this.$emit('hover', event);
    }
  }
}
</script>

<style scoped>
.comparison-container {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1.5rem;
}

.message-panel {
  border: 1px solid #ddd;
  border-radius: 4px;
  padding: 1rem;
  background-color: white;
}

.message-panel h3 {
  margin-top: 0;
  padding-bottom: 0.5rem;
  border-bottom: 1px solid #eee;
  color: #333;
}

.original {
  border-left: 4px solid #4a90e2;
}

.deidentified {
  border-left: 4px solid #50b83c;
}

@media (max-width: 768px) {
  .comparison-container {
    grid-template-columns: 1fr;
  }
}
</style>