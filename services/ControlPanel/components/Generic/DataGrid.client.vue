<script setup lang="ts">

import {
    DxColumn,
    DxColumnChooser,
    DxDataGrid,
    DxExport,
    DxFilterRow,
    DxGroupPanel,
    DxLookup,
    DxPaging,
    DxSummary,
    DxTotalItem,
    DxSelection,
    DxStateStoring,
    type DxDataGridTypes,
} from 'devextreme-vue/data-grid';
import type { ColumnConfig } from '@/models/DatagridModels';
import { exportDataGrid } from 'devextreme/excel_exporter';
import { saveAs } from 'file-saver';
import { Workbook } from "exceljs";

const props = defineProps({
    dataSet: {
        type: Array as PropType<Array<any> | null>,
    },
    columns: {
        type: Array as PropType<Array<ColumnConfig>>,
        required: true
    },
    showSummary: {
        type: Boolean,
        default: false
    },
    rowPrepared: {
        type: Function as PropType<(e: any) => void>,
        required: false,
        default: () => {
        }
    },
    hasActions: {
        type: Boolean,
    },
    hasImage: {
        type: Boolean,
    },
    showExport: {
        type: Boolean,
        default: true
    },
    actionsColumnName: {
        type: String,
        default: "Ενέργειες"
    },
    actionsColumnWidth: {
        type: Number,
        default: 120
    },
    wordWrapEnabled: {
        type: Boolean,
        default: false
    },
    hoverStateEnabled: {
        type: Boolean,
        default: true
    },
    showColumnLines: {
        type: Boolean,
        default: true
    },
    showRowLines: {
        type: Boolean,
        default: true
    },
    pageSize: {
        type: Number,
        default: 10
    },
    imageColumnName: {
        type: String,
        default: 'Φωτογραφία'
    },
    imageWidth: {
        type: Number,
    },
    allowGrouping: {
        type: Boolean,
        default: true
    },
    hasSelector: {
        type: Boolean,
        default: false
    },
    selectedRows: {
        type: Array as PropType<string[]>,
        default: () => []
    },
    storageName: {
        type: String,
        default: ''
    }
});

const columnsToRender = computed(() => {
    return props.columns;
});

const showTooltipTemplate = (datafield: string) => {
    const statement = props.dataSet?.some(x => x.ToolTips && x.ToolTips.some((y: {
        inColumn: string;
    }) => y.inColumn == datafield));
    return statement;
};

const dataGridRef = ref();

const getDataSource = async () => {
    const instance = dataGridRef.value?.instance;

    const filterExpr = instance.getCombinedFilter(true);

    let dataSource = await instance.getDataSource().store().load({
        filter: filterExpr
    });

    return dataSource;
};

const getColumnDataType = (type: string) => {
    if (type == 'email')
        return 'string';
    if (type == 'currency')
        return 'number';
    if (type === 'color') {
        return 'string';
    }
    return type
};

const getColumnFormat = (columType: string) => {
    if (columType == 'date')
        return 'dd/MM/yyyy';

    if (columType == 'datetime')
        return 'dd/MM/yyyy HH:mm:ss';

    if (columType == 'currency')
        return {
            style: 'currency',
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
            currency: 'EUR',
            currencyDisplay: 'symbol'
        };

    return null
};

const getCellTemplate = (columnDataField: string, columnType: string) => {

    if (showTooltipTemplate(columnDataField)) {
        return 'toolTipcellTemplate';
    }

    if (columnType === 'email') {
        return 'emailCellTemplate';
    }

    if (columnType === 'color') {
        return 'colorCellTemplate';
    }

    return null;
};

const columnsWithSummary = computed(() => {
    return props.columns.filter(x => x.summaryType);
});

const onExporting = async (e: any) => {
    const workbook = new Workbook();
    const worksheet = workbook.addWorksheet('Main sheet');

    await exportDataGrid({
        component: e.component,
        worksheet: worksheet,
        autoFilterEnabled: true,
    });

    workbook.xlsx.writeBuffer().then((buffer) => {
        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'DataGrid.xlsx');
    });
};

const getSelected = <T>() => {
    return dataGridRef.value?.instance.getSelectedRowsData() as T[];
};

const setSelected = () => {
    dataGridRef.value?.instance.selectRows(props.selectedRows, false);
};

defineExpose({
    getDataSource,
    getSelected,
    dataGridRef
});
</script>
<template>
    <DxDataGrid ref="dataGridRef" :allow-column-resizing="true" :data-source="props.dataSet" key-expr="id"
        :show-borders="true" :word-wrap-enabled="props.wordWrapEnabled" :allow-column-reordering="true"
        @row-prepared="props.rowPrepared" :hover-state-enabled="hoverStateEnabled" :show-column-lines="showColumnLines"
        :show-row-lines="showRowLines" @exporting="onExporting" @content-ready="setSelected">

        <DxFilterRow :visible="true" />
        <DxGroupPanel :visible="props.allowGrouping" />
        <DxExport :enabled="props.showExport" />
        <DxPaging :page-size="pageSize" />
        <DxColumnChooser :enabled="true" />
        <DxSelection v-if="hasSelector" select-all-mode="allPages" show-check-boxes-mode="always" mode="multiple" />
        <DxStateStoring :enabled="storageName.length != 0" type="localStorage" :storage-key="storageName" />

        <DxColumn v-if="hasImage" cell-template="imageCellTemplate" :allow-exporting="false"
            :caption="props.imageColumnName" :width="props.imageWidth" />

        <template #imageCellTemplate="{ data }">
            <slot name="image" :data="data.data" />
        </template>

        <template #colorCellTemplate="{ data }">
            <div :style="`background-color:${data.value};padding:.5rem`">{{ data.value }}</div>
        </template>

        <DxColumn v-for="column in columnsToRender" :key="column.dataField" :data-field="column.dataField"
            :caption="column.caption" :allow-sorting="column.allowSorting" :width="column.width"
            :data-type="getColumnDataType(column.type)" :filter-value="column.filter"
            :format="getColumnFormat(column.type)" :cell-template="getCellTemplate(column.dataField, column.type)"
            :visible="column.visible" :sort-order="column.sortOrder" :calculate-cell-value="column.calculateCellValue">
            <DxLookup v-if="column.lookup?.dataSource" :data-source="column.lookup?.dataSource" />
        </DxColumn>

        <template #emailCellTemplate="{ data }">
            <a :href="`mailto:${data.value}`">{{ data.value }}</a>
        </template>

        <DxColumn v-if="hasActions" cell-template="actionsCellTemplate" :allow-exporting="false"
            :caption="props.actionsColumnName" :width="props.actionsColumnWidth" />

        <template #actionsCellTemplate="{ data }">
            <slot name="actions" :data="data.data" />
        </template>

        <DxSummary v-if="props.showSummary">
            <DxTotalItem :summary-type="column.summaryType" :column="column.dataField"
                v-for="column in columnsWithSummary" display-format="Σύνολο: {0}" />
        </DxSummary>
    </DxDataGrid>
</template>
