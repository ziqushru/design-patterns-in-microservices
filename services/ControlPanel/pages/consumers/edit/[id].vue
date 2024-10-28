<script setup lang="ts">

useHead({
    title: 'Επεξεργασία - Καταναλωτές - ContractCon',
});

definePageMeta({
    layout: 'admin'
});

</script>

<template>
    <div class="container">
        <ConsumerForm mode="edit" />
    </div>
</template>

<style scoped lang="scss">
.container {
    margin-right: 15rem;
}
</style>
