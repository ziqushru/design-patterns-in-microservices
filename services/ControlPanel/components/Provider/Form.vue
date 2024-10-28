<script setup lang="ts">

import type { PropType } from 'vue';
import { useProviders } from '@/composables/endpoints/useProviders';
import type { Provider } from '@/models/Provider';

const route = useRoute();
const providersHttpClient = useProviders();

const props = defineProps({
    mode: String as PropType<'edit' | 'create'>
});

const shouldRender = ref<boolean>(false);
const provider = ref<Provider>(
    {
        id: '',
        brandName: '',
        vat: '',
        email: '',
    }
);

const handleSubmit = async (value: Provider) => {
    if (props.mode === 'create') {
        const response = await providersHttpClient.create(value);

        if (response.success) {
            navigateTo('/providers');
        } else {
            ElMessage.error(response.message);
        }
    } else {
        const response = await providersHttpClient.update(value);

        if (response.success) {
            navigateTo('/providers');
        } else {
            ElMessage.error(response.message);
        }
    }
}

onMounted(async () => {
    if (props.mode === 'edit') {
        const response = await providersHttpClient.getById(route.params.id.toString());

        if (response.success) {
            provider.value = response.data;
        }
    }
})

</script>

<template>
    <div class="navigation">
        <v-btn to="/providers">Επιστροφή</v-btn>
    </div>

    <FormKit type="form" v-if="shouldRender" #default="{ value }" :actions="false" @submit="handleSubmit">

        <FormKit type="hidden" name="id" :value="route.params.id?.toString()" />

        <FormKit type="text" label="Επωνυμία" name="brandName" validation="required" :value="provider.brandName" />
        <FormKit type="text" label="ΑΦΜ" name="vat" validation="required" :value="provider.vat" />
        <FormKit type="text" label="Email" name="email" validation="required" :value="provider.email" />

        <FormKit type="submit" label="Υποβολή" />
    </FormKit>
</template>

<style scoped>
.navigation {
    margin: 1rem 0;
}
</style>
