<script setup lang="ts">
import HtmlEditor from "@/components/Generic/HtmlEditor.vue";

const props = defineProps<{
    name: string,
    value: string,
    label?: string
}>();

const htmlHiddenInput = ref<{
    node: {
        input: Function
    }
} | null>(null);

const updateInputValue = (value: string): void => {
    if (htmlHiddenInput.value?.node) {
        htmlHiddenInput.value.node.input(value);
    }
}
</script>

<template>
    <div>
        <b>{{ props.label }}</b>
        <FormKit :name="name" :value="value" type="hidden" ref="htmlHiddenInput" />
        <HtmlEditor :initial-value="props.value" @update-value="updateInputValue" />
    </div>
</template>

<style scoped lang="scss"></style>
