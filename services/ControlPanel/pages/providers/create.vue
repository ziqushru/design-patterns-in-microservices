<script setup lang="ts">

import { ProviderForm } from '#build/components';

useHead({
    title: 'Δημιουργία - Πάροχοι - ContractCon',
});

definePageMeta({
    layout: 'admin'
});

</script>

<template>
    <div class="container">
        <ProviderForm mode="create" />
    </div>
</template>

<style scoped lang="scss">
.container {
    margin-right: 15rem;
}
</style>
