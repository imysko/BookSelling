<script setup>
import useAuthenticationStore from '@/stores/authenticationStore'
import {FontAwesomeIcon} from '@fortawesome/vue-fontawesome'
import {computed, ref} from 'vue'

const props = defineProps({
  book: Object
})

const bookCount = ref(1)

const formatGenries = computed(() => {
  return props.book.booksGenres.map(bg => bg.genre.name).join(', ')
})

const authStore = useAuthenticationStore()
const emit = defineEmits(['active-book-click', 'edit-book-click', 'buy-book-click'])

function onActiveClick(book, active) {
  emit("active-book-click", book, active)
}

function onEditClick(book) {
  emit("edit-book-click", book)
}

function onBuyClick(book) {
  emit("buy-book-click", book.id, bookCount.value)
  bookCount.value = 1
}
</script>

<template>
  <div>
    <b-card
        no-body
        tag="article"
        style="width: 220px"
        :border-variant="props.book.active ? 'grey' : 'secondary'"
        class="m-2">

      <b-card-img
          :src="props.book.image"
          alt="Image"
          width="200"
          height="300"
          style="object-fit: cover;"
          top>
      </b-card-img>

      <b-card-body class="pt-2 pb-0">
        <b-card-text class="book-title">{{ props.book.name }}</b-card-text>
        <b-card-text class="book-genries">{{ formatGenries }}</b-card-text>

        <div class="d-flex justify-content-sm-between">
          <b-card-text>{{ props.book.author }}</b-card-text>
          <b-card-text class="book-year">{{ props.book.year }} г.</b-card-text>
        </div>

        <div class="d-flex justify-content-sm-between">
          <b-card-text>На складе: {{ props.book.storeCount }}</b-card-text>
          <b-card-text class="book-price">{{ props.book.price }} ₽</b-card-text>
        </div>
      </b-card-body>

      <b-card-footer v-if="authStore.canAction('user') || authStore.canAction('editor')">
        <div v-if="authStore.canAction('user')">
          <div v-if="props.book.storeCount === 0">
            Книга закончились
          </div>
          <div v-else-if="!props.book.active">
            Книга недоступна для покупки
          </div>
          <div v-else class="d-flex justify-content-sm-between">
            <b-form-spin-button v-model="bookCount" min="1" :max="props.book.storeCount" inline/>
            <b-button pill class="fab fa-shopify" variant="outline-warning" @click="onBuyClick(props.book)"/>
          </div>
        </div>

        <div v-if="authStore.canAction('editor')" class="d-flex justify-content-end">
          <b-button pill class="me-2" variant="outline-warning" @click="onEditClick({...props.book})">
            <FontAwesomeIcon icon="fas fa-pen me-2" size="sm" />
          </b-button>
          <b-button v-if="props.book.active" pill variant="outline-danger" @click="onActiveClick(props.book, false)">
            <FontAwesomeIcon icon="fas fa-trash" size="sm" />
          </b-button>
          <b-button v-else pill variant="outline-success" @click="onActiveClick(props.book, true)">
            <FontAwesomeIcon icon="fas fa-rotate-left" size="sm" />
          </b-button>
        </div>
      </b-card-footer>
    </b-card>
  </div>
</template>

<style scoped>
.book-title {
  font-size: 16px;
  font-weight: bold;
}
.book-genries {
  text-align: center;
  font-size: 16px;
  font-weight: normal;
  font-style: italic;
}
.book-year {
  font-style: italic;
}
.book-price {
  font-weight: bold;
  font-style: italic;
}
</style>