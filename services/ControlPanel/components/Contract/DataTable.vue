<script setup lang="ts">

import type { ColumnConfig } from '../../models/DatagridModels';
import type { Contract } from '@/models/Contract';
import { useContracts } from "@/composables/endpoints/useContracts";

const contractsHttpClient = useContracts();

const dataSet = ref<Array<Contract>>();

const emit = defineEmits<(e: 'onDeleteClick') => void>();

const columns = computed((): Array<ColumnConfig> => {
    return [
        {
            dataField: 'consumerFullName',
            caption: 'Όνομα Καταναλωτή',
            allowSorting: true,
            width: null,
            type: 'string',
            visible: true,
            sortOrder: 'desc'
        },
        {
            dataField: 'providerFullName',
            caption: 'Όνομα Παρόχου',
            allowSorting: true,
            width: null,
            type: 'string',
            visible: true,
            sortOrder: 'desc'
        },
        {
            dataField: 'status',
            caption: 'Κατάσταση',
            allowSorting: true,
            width: null,
            type: 'string',
            visible: true,
            sortOrder: 'desc'
        }
    ]
});

const getContracts = async () => {
    const response = await contractsHttpClient.getAll();

    if (!response.success) {
        ElMessage.error(response.message);
        return;
    }

    dataSet.value = response.data;
}

const handleDelete = async (id: string) => {
    ElMessageBox.confirm(
        'Η εγγραφή πρόκειται να διαγραφεί, συνέχεια;',
        'Προσοχή',
        {
            confirmButtonText: 'Συνέχεια',
            cancelButtonText: 'Ακύρωση',
            type: 'warning',
        }
    ).then(async () => {
        const response = await contractsHttpClient.remove(id);

        if (!response.success) {
            ElMessage.error(response.message);
        }

        emit('onDeleteClick');
    });

}

onMounted(async () => {
    await getContracts();
});

defineExpose({
    getItems: getContracts
});

</script>

<template>
    <GenericDataGrid :columns="columns" :data-set="dataSet" :has-actions="true" :actions-column-width="100"
        :storage-name="'contractsStorage'">
        <template #actions="{ data }">
            <div class="d-flex justify-space-around">
                <v-menu>
                    <template v-slot:activator="{ props }">
                        <v-btn class="ma-2" color="blue" icon="mdi-format-list-bulleted" v-bind="props">
                        </v-btn>
                    </template>

                    <v-list>
                        <v-list-item :to="`/contracts/edit/${data.id}`">
                            <v-list-item-title>Επεξεργασία</v-list-item-title>
                        </v-list-item>

                        <v-list-item @click="handleDelete(data.id)">
                            <v-list-item-title>Διαγραφή</v-list-item-title>
                        </v-list-item>
                    </v-list>
                </v-menu>
            </div>
        </template>
    </GenericDataGrid>
</template>

<style scoped>
.action-buttons {
    display: flex;
    justify-content: space-between;
    align-items: center;
}
</style>
