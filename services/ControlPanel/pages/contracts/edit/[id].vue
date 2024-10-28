<script setup lang="ts">

useHead({
    title: 'Επεξεργασία - Συμβάσεις - ContractCon',
});

definePageMeta({
    layout: 'admin'
});

</script>

<template>
    <div class="container">
        <ContractForm mode="edit" />
    </div>
</template>

<style scoped lang="scss">
.container {
    margin-right: 15rem;
}
</style>
