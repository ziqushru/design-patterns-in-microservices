<script setup lang="ts">

useHead({
    title: 'Επεξεργασία - Πάροχοι - ContractCon',
});

definePageMeta({
    layout: 'admin'
});

</script>

<template>
    <div class="container">
        <ProviderForm mode="edit" />
    </div>
</template>

<style scoped lang="scss">
.container {
    margin-right: 15rem;
}
</style>
