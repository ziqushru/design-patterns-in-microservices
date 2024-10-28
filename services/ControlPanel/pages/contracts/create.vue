<script setup lang="ts">

useHead({
    title: 'Δημιουργία - Συμβάσεις - ContractCon',
});

definePageMeta({
    layout: 'admin'
});

</script>

<template>
    <div class="container">
        <ContractForm mode="create" />
    </div>
</template>

<style scoped lang="scss">
.container {
    margin-right: 15rem;
}
</style>
