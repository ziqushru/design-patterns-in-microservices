export type ColumnConfig = {
    dataField: string;
    caption: string;
    allowSorting: boolean;
    width: number | string | null;
    type: 'number' | 'boolean' | 'string' | 'date' | 'datetime' | 'email' | 'currency' | 'color';
    lookup?: {
        dataSource?: string[];
    };
    filter?: string;
    visible: boolean;
    sortOrder?: 'asc' | 'desc';
    summaryType?: 'sum' | 'count';
    calculateCellValue?:(row:any) => any;
}
