<script setup lang="ts">

import { ConsumerTypeOptions, type Consumer } from '@/models/Consumer';
import type { ColumnConfig } from '@/models/DatagridModels';
import { useConsumers } from "@/composables/endpoints/useConsumers";

const consumersHttpClient = useConsumers();

const emit = defineEmits<(e: 'onEditClick', id: string) => void>();

const dataSet = ref<Array<Consumer>>();

const columns = computed((): Array<ColumnConfig> => {
    return [
        {
            dataField: 'firstName',
            caption: 'Όνομα',
            allowSorting: true,
            width: null,
            type: 'string',
            visible: true,
        },
        {
            dataField: 'lastName',
            caption: 'Επώνυμο',
            allowSorting: true,
            width: null,
            type: 'string',
            visible: true,
        },
        {
            dataField: 'email',
            caption: 'Email',
            allowSorting: true,
            width: null,
            type: 'string',
            visible: true,
        },
        {
            dataField: 'cellPhone',
            caption: 'Κινητό',
            allowSorting: true,
            width: null,
            type: 'string',
            visible: true,
        },
        {
            dataField: 'type',
            caption: 'Τύπος',
            allowSorting: true,
            width: null,
            type: 'string',
            visible: true,
            calculateCellValue: (data: Consumer) => {
                return ConsumerTypeOptions[data.type].label;
            }
        }
    ]
});

const getConsumers = async () => {
    const response = await consumersHttpClient.getAll();

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
        const response = await consumersHttpClient.remove(id);

        if (!response.success) {
            ElMessage.error(response.message);
        }

        await getConsumers();
    });
}

onMounted(async () => {
    await getConsumers();
});

</script>

<template>
    <GenericDataGrid :columns="columns" :data-set="dataSet" :has-actions="true" :actions-column-width="100"
        :storage-name="'consumersStorage'">
        <template #actions="{ data }">
            <div class="d-flex justify-space-around">
                <v-menu>
                    <template v-slot:activator="{ props }">
                        <v-btn class="ma-2" color="blue" icon="mdi-format-list-bulleted" v-bind="props">
                        </v-btn>
                    </template>

                    <v-list>
                        <v-list-item :to="`/consumers/edit/${data.id}`">
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
