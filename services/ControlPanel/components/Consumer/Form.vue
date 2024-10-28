<script setup lang="ts">

import type { PropType } from 'vue';
import { ConsumerType, ConsumerTypeOptions, type Consumer } from '@/models/Consumer';
import { useConsumers } from "@/composables/endpoints/useConsumers";

const route = useRoute();
const consumersHttpClient = useConsumers();

const props = defineProps({
    mode: String as PropType<'edit' | 'create'>
});

const shouldRender = ref<boolean>(false);

const consumer = ref<Consumer>(
    {
        id: '',
        firstName: '',
        lastName: '',
        email: '',
        cellPhone: '',
        type: ConsumerType.NotDefined
    }
);

const handleSubmit = async (value: Consumer) => {
    if (props.mode === 'create') {
        const response = await consumersHttpClient.create(value);

        if (response.success) {
            navigateTo('/consumers');
        } else {
            ElMessage.error(response.message);
        }
    } else {

        const response = await consumersHttpClient.update(value);

        if (response.success) {
            navigateTo('/consumers');
        } else {
            ElMessage.error(response.message);
        }
    }
}

onMounted(async () => {
    if (props.mode === 'edit') {
        const response = await consumersHttpClient.getById(route.params.id.toString());

        if (response.success) {
            consumer.value = response.data;
            shouldRender.value = true;
        }
    } else {
        shouldRender.value = true;
    }
})

</script>

<template>
    <div class="navigation">
        <v-btn to="/consumers">Επιστροφή</v-btn>
    </div>

    <FormKit type="form" v-if="shouldRender" #default="{ value }" :actions="false"
        @submit="handleSubmit">

        <FormKit type="hidden" name="id" :value="route.params.id?.toString()" />

        <FormKit type="text" label="Όνομα" name="firstName" validation="required" :value="consumer.firstName" />
        <FormKit type="text" label="Επώνυμο" name="lastName" validation="required" :value="consumer.lastName" />
        <FormKit type="text" label="Email" name="email" validation="required" :value="consumer.email" />
        <FormKit type="text" label="Κινητό" name="cellPhone" validation="required" :value="consumer.cellPhone" />
        <FormKit type="select" label="Τύπος" name="type" validation="required" :options="ConsumerTypeOptions" />

        <FormKit type="submit" label="Υποβολή" />
    </FormKit>
</template>

<style scoped lang="scss">
.navigation {
    margin: 1rem 0;
}
</style>
