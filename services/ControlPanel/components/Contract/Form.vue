<script setup lang="ts">

import type { PropType } from 'vue';
import { ContractStatus, type Contract } from '@/models/Contract';
import { useContracts } from "@/composables/endpoints/useContracts";
import { ConsumerType } from '~/models/Consumer';

const route = useRoute();
const contractsHttpClient = useContracts();

const props = defineProps({
    mode: String as PropType<'edit' | 'create'>
});

const shouldRender = ref<boolean>(false);
const contract = ref<Contract>(
    {
        id: '',
        status: ContractStatus.NotDefined,
        consumerId: '',
        providerId: '',
        consumer: {
            id: '',
            firstName: '',
            lastName: '',
            email: '',
            cellPhone: '',
            type: ConsumerType.NotDefined
        },
        provider: {
            id: '',
            brandName: '',
            vat: '',
            email: '',
        },
    }
);

const handleSubmit = async (value: Contract) => {
    if (props.mode === 'create') {
        const response = await contractsHttpClient.create(value);

        if (response.success) {
            navigateTo('/contracts');
        } else {
            ElMessage.error(response.message);
        }
    } else {
        const response = await contractsHttpClient.update(value);

        if (response.success) {
            navigateTo('/contracts');
        } else {
            ElMessage.error(response.message);
        }
    }
}

onMounted(async () => {
    if (props.mode === 'edit') {
        const response = await contractsHttpClient.getById(route.params.id.toString());

        if (response.success) {
            contract.value = response.data;
        }
    }
})

</script>

<template>
    <div class="navigation">
        <v-btn to="/contracts">Επιστροφή</v-btn>
    </div>
    <FormKit type="form" v-if="shouldRender" #default="{ value }" :actions="false" @submit="handleSubmit">

        <FormKit type="hidden" name="id" :value="route.params.id?.toString()" />

        <FormKit type="text" label="Επώνυμο Καταναλωτή" name="brandName" validation="required"
            :value="contract.consumer.lastName" />
        <FormKit type="text" label="Επωνυμία Παρόχου" name="email" validation="required"
            :value="contract.provider.brandName" />

        <FormKit type="submit" label="Υποβολή" />
    </FormKit>
</template>

<style scoped>
.navigation {
    margin: 1rem 0;
}
</style>
