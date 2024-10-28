<script setup lang="ts">

import type { ColumnConfig } from '../../models/DatagridModels';
import type { Provider } from '@/models/Provider';
import { useProviders } from "@/composables/endpoints/useProviders";

const providersHttpClient = useProviders();

const dataSet = ref<Array<Provider>>();

const columns = computed((): Array<ColumnConfig> => {
    return [
        {
            dataField: 'brandName',
            caption: '',
            allowSorting: true,
            width: null,
            type: 'string',
            visible: true,
            sortOrder: 'desc'
        },
        {
            dataField: 'vat',
            caption: 'ΑΦΜ',
            allowSorting: true,
            width: null,
            type: 'string',
            visible: true,
            sortOrder: 'desc'
        },
        {
            dataField: 'email',
            caption: 'Email',
            allowSorting: true,
            width: null,
            type: 'string',
            visible: true,
            sortOrder: 'desc'
        }
    ]
});

const getItems = async () => {
    const response = await providersHttpClient.getAll();

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
        const response = await providersHttpClient.remove(id);

        if (!response.success) {
            ElMessage.error(response.message);
        }

        await getItems();
    });

}

onMounted(async () => {
    await getItems();
});

</script>

<template>
    <GenericDataGrid :columns="columns" :data-set="dataSet" :has-actions="true" :actions-column-width="100"
        :storage-name="'providersStorage'">
        <template #actions="{ data }">
            <div class="d-flex justify-space-around">
                <v-menu>
                    <template v-slot:activator="{ props }">
                        <v-btn class="ma-2" color="blue" icon="mdi-format-list-bulleted" v-bind="props">
                        </v-btn>
                    </template>

                    <v-list>
                        <v-list-item :to="`/providers/edit/${data.id}`">
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
