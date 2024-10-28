<script setup lang="ts">

useHead({
    title: 'Δημιουργία - Καταναλωτές - ContractCon',
});

definePageMeta({
    layout: 'admin'
});

</script>

<template>
    <div class="container">
        <ConsumerForm mode="create" />
    </div>
</template>

<style scoped lang="scss">
.container {
    margin-right: 15rem;
}
</style>
