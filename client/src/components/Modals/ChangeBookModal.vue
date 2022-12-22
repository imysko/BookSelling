<script setup>
import BModal from '@/components/Modals/BModal.vue'
import { ref } from 'vue'

const book = ref(Object)
const modalRef = ref(null)

const emit = defineEmits(['book-edit'])

function onSubmit() {
  book.value.price = parseInt(book.value.price)
  book.value.year = parseInt(book.value.year)
  book.value.storeCount = parseInt(book.value.storeCount)

  if (isNaN(book.value.price) || 100 <= book.value.price && book.value.price <= 20000 &&
      isNaN(book.value.year) || 1700 <= book.value.year && book.value.year <= 2022 &&
      isNaN(book.value.storeCount) || 0 <= book.value.storeCount && book.value.storeCount <= 150) {
    emit('book-edit', book.value)
    modalRef.value.close()
  }
}

defineExpose({
  show () {
    modalRef.value.show()
  },
  handBook (object) {
    book.value = object
  },
})
</script>

<template>
  <BModal ref="modalRef">
    <div class="modal-dialog modal-lg">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Изменение {{ book.name }}</h5>
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
                    v-model="book.name"
                    type="text"
                    class="form-control"
                    placeholder="name">
                <label for="floatingInput">Наименование</label>
              </div>
            </b-col>

            <b-col sm="6">
              <div class="form-floating">
                <input
                    v-model="book.author"
                    type="text"
                    class="form-control"
                    placeholder="author">
                <label for="floatingInput">Автор</label>
              </div>
            </b-col>

            <b-col sm="4">
              <div class="form-floating">
                <input
                    v-model="book.year"
                    type="number"
                    min="1700"
                    max="2022"
                    class="form-control"
                    placeholder="year">
                <label for="floatingInput">Год издания</label>
              </div>
            </b-col>

            <b-col sm="4">
              <div class="form-floating">
                <input
                    v-model="book.price"
                    type="number"
                    min="100"
                    max="20000"
                    class="form-control"
                    placeholder="price">
                <label for="floatingInput">Цена ₽</label>
              </div>
            </b-col>

            <b-col sm="4">
              <div class="form-floating">
                <input
                    v-model="book.storeCount"
                    type="number"
                    min="0"
                    max="150"
                    class="form-control"
                    placeholder="storeCount">
                <label for="floatingInput">Количество на складе</label>
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