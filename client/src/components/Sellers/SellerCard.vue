<script setup>
import useAuthenticationStore from '@/stores/authenticationStore'
import {FontAwesomeIcon} from '@fortawesome/vue-fontawesome'

const authStore = useAuthenticationStore()

const emit = defineEmits(['active-seller-click', 'edit-seller-click'])

const props = defineProps({
  seller: Object
})

function onActiveClick(seller, active) {
  emit("active-seller-click", seller, active)
}

function onEditClick(seller) {
  emit('edit-seller-click', seller)
}
</script>

<template>
  <b-card
      :title="`${props.seller.name} ${props.seller.surname}`"
      :border-variant="props.seller.active ? 'grey' : 'secondary'"
      class="m-2">
    <b-card-subtitle>
      {{ props.seller.fname }}
    </b-card-subtitle>

    <b-card-text>
      <b>Телефон:</b> {{ props.seller.phoneNumber }} <br />
    </b-card-text>

    <b-card-text v-if="props.seller.active">
      <b-button variant="outline-success" pill>Активный</b-button>
    </b-card-text>
    <b-card-text v-else>
      <b-button variant="outline-secondary" pill>Отстранён</b-button>
    </b-card-text>

    <b-button
        pill
        variant="outline-warning"
        class="me-1"
        @click="onEditClick({ ...props.seller })">
      <FontAwesomeIcon icon="fas fa-pen" size="sm" />
    </b-button>

    <b-button
        pill
        variant="outline-danger"
        @click="onActiveClick(props.seller, false)"
        v-if="props.seller.active">
      <FontAwesomeIcon icon="fas fa-trash" size="sm" />
    </b-button>

    <b-button
        pill
        variant="outline-success"
        @click="onActiveClick(props.seller, true)"
        v-else>
      <FontAwesomeIcon icon="fas fa-rotate-left" size="sm" />
    </b-button>
  </b-card>
</template>

<style scoped>
</style>