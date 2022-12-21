<script setup>
import BModal from '@/components/Modals/BModal.vue'
import {ref} from 'vue'

const modalRef = ref(Object)
const emit = defineEmits(['active'])

const currentSeller = ref(Object)

defineExpose({
  show () {
    modalRef.value.show()
  },
  handSeller (seller) {
    currentSeller.value = seller
  }
})
</script>

<template>
  <BModal ref="modalRef">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 v-if="currentSeller.active" class="modal-title">Отстранить сотрудника?</h5>
          <h5 v-else class="modal-title">Вернуть сотрудника?</h5>

          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body">
          <p v-if="currentSeller.active">Вы уверены что хотите отстранить сотрудника?</p>
          <p v-else>Вы уверены что хотите вернуть сотрудника?</p>
        </div>
        <div class="modal-footer">
          <b-button
              type="button"
              variant="danger"
              data-bs-dismiss="modal"
              @click="$emit('active', currentSeller.id, !currentSeller.active)">
            Да
          </b-button>
          <b-button type="button" variant="outline-secondary" data-bs-dismiss="modal">Отмена</b-button>
        </div>
      </div>
    </div>
  </BModal>
</template>

<style scoped>
</style>