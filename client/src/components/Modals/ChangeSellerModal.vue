<script setup>
import BModal from '@/components/Modals/BModal.vue'
import { ref } from 'vue'

const seller = ref(Object)
const modalRef = ref(null)

const emit = defineEmits(['seller-edit'])

function onSubmit() {
  if (seller.value.phoneNumber === '') {
    seller.value.phoneNumber = null
  }

  if (89000000000 <= seller.value.phoneNumber && seller.value.phoneNumber <= 89999999999 || seller.value.phoneNumber === null) {
    emit('seller-edit', seller.value)
    modalRef.value.close()
  }
}

defineExpose({
  show () {
    modalRef.value.show()
  },
  handSeller (object) {
    seller.value = object
  },
})
</script>

<template>
  <BModal ref="modalRef">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Изменение {{ seller.name }} {{ seller.surname }}</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>

        <div class="modal-body">
          <form
              ref="editForm"
              @submit.prevent="onSubmit"
              method="POST"
              class="row g-3 justify-content-sm-between">

            <b-col sm="6">
              <div class="form-floating">
                <input
                    v-model="seller.name"
                    type="text"
                    class="form-control"
                    placeholder="name">
                <label for="floatingInput">Имя</label>
              </div>
            </b-col>

            <b-col sm="6">
              <div class="form-floating">
                <input
                    v-model="seller.surname"
                    type="text"
                    class="form-control"
                    placeholder="surname">
                <label for="floatingInput">Фамилия</label>
              </div>
            </b-col>

            <b-col sm="6">
              <div class="form-floating">
                <input
                    v-model="seller.fname"
                    type="text"
                    class="form-control"
                    placeholder="fname">
                <label for="floatingInput">Отчество</label>
              </div>
            </b-col>

            <b-col sm="6">
              <div class="form-floating">
                <input
                    v-model="seller.phoneNumber"
                    type="number"
                    min="89000000000"
                    max="89999999999"
                    class="form-control"
                    placeholder="phoneNumber">
                <label for="floatingInput">Номер телефона</label>
              </div>
            </b-col>

            <b-col sm="12" class="text-end">
              <b-button type="submit" variant="outline-warning">Сохранить</b-button>
              <b-button class="ms-2" type="button" variant="outline-secondary" data-bs-dismiss="modal">Отмена</b-button>
            </b-col>
          </form>
        </div>
      </div>
    </div>
  </BModal>
</template>

<style scoped>
</style>