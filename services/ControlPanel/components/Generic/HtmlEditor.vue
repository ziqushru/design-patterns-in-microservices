<script setup lang="ts">
import { ref } from 'vue';
import {
    DxHtmlEditor,
    DxToolbar,
    DxMediaResizing,
    DxImageUpload,
    DxItem,
} from 'devextreme-vue/html-editor';

const props = defineProps<{
    initialValue: string;
}>()

const sizeValues = ['8pt', '10pt', '12pt', '14pt', '18pt', '24pt', '36pt'];
const fontValues = ['Arial', 'Courier New', 'Georgia', 'Impact', 'Lucida Console', 'Tahoma', 'Times New Roman', 'Verdana'];
const headerValues = [false, 1, 2, 3, 4, 5];
const currentTabs = ref([
    { name: 'From This Device', value: ['file'] },
    { name: 'From the Web', value: ['url'] },
    { name: 'Both', value: ['file', 'url'] },
]);
const markup = ref('');
const isMultiline = ref(true);
const fontSizeOptions = { inputAttr: { 'aria-label': 'Font size' } };
const fontFamilyOptions = { inputAttr: { 'aria-label': 'Font family' } };
const headerOptions = { inputAttr: { 'aria-label': 'Font family' } };

const emits = defineEmits<{
    (e: 'updateValue', value: string): void
}>();

watch(markup, (newValue) => {
    emits('updateValue', newValue);
});

onMounted(() => {
    markup.value = props.initialValue;
})
</script>
<template>
    <div>
        <DxHtmlEditor v-model="markup" height="30rem">
            <DxMediaResizing :enabled="true" />
            <DxImageUpload :tabs="currentTabs" file-upload-mode="base64" />
            <DxToolbar :multiline="isMultiline">
                <DxItem name="undo" />
                <DxItem name="redo" />
                <DxItem name="separator" />
                <DxItem name="size" :accepted-values="sizeValues" :options="fontSizeOptions" />
                <DxItem name="font" :accepted-values="fontValues" :options="fontFamilyOptions" />
                <DxItem name="separator" />
                <DxItem name="bold" />
                <DxItem name="italic" />
                <DxItem name="strike" />
                <DxItem name="underline" />
                <DxItem name="separator" />
                <DxItem name="alignLeft" />
                <DxItem name="alignCenter" />
                <DxItem name="alignRight" />
                <DxItem name="alignJustify" />
                <DxItem name="separator" />
                <DxItem name="orderedList" />
                <DxItem name="bulletList" />
                <DxItem name="separator" />
                <DxItem name="header" :accepted-values="headerValues" :options="headerOptions" />
                <DxItem name="separator" />
                <DxItem name="color" />
                <DxItem name="background" />
                <DxItem name="separator" />
                <DxItem name="link" />
                <DxItem name="image" />
                <DxItem name="separator" />
                <DxItem name="clear" />
                <DxItem name="codeBlock" />
                <DxItem name="blockquote" />
                <DxItem name="separator" />
                <DxItem name="insertTable" />
                <DxItem name="deleteTable" />
                <DxItem name="insertRowAbove" />
                <DxItem name="insertRowBelow" />
                <DxItem name="deleteRow" />
                <DxItem name="insertColumnLeft" />
                <DxItem name="insertColumnRight" />
                <DxItem name="deleteColumn" />
            </DxToolbar>
        </DxHtmlEditor>
    </div>
</template>
<style scoped lang="scss">
div {
    margin: 1rem 0;
}

.dx-htmleditor-content img {
    vertical-align: middle;
    padding-right: 10px;
}

.dx-htmleditor-content table {
    width: 50%;
}

.options {
    padding: 20px;
    background-color: rgba(191, 191, 191, 0.15);
    margin-top: 20px;
}

.caption {
    font-size: 18px;
    font-weight: 500;
}

.option {
    margin-top: 10px;
    display: inline-block;
    margin-right: 40px;
}

.option>.dx-selectbox,
.option>.label {
    display: inline-block;
    vertical-align: middle;
}

.option>.label {
    margin-right: 10px;
}
</style>
