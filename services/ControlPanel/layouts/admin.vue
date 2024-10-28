<script setup lang="ts">

const drawer = ref(true);

useHead({
    htmlAttrs: {
        class: 'dark'
    }
})
</script>

<template>
    <v-app>
        <v-navigation-drawer v-model="drawer">
            <v-list-item>
                <v-row justify="center">
                    <v-avatar size="120" class="mt-3 mb-8 mt-8" image="/img/batman-avatar.png" />
                </v-row>
            </v-list-item>
            <v-divider class="mb-4"></v-divider>
            <v-list dense nav color="purple-lighten-2">
                <v-list-item link to="/" prepend-icon="mdi-home-outline">
                    <v-list-item-title>Αρχική Σελίδα</v-list-item-title>
                </v-list-item>
                <v-list-item link to="/consumers" prepend-icon="mdi-account-box-outline">
                    <v-list-item-title>Καταναλωτές</v-list-item-title>
                </v-list-item>
                <v-list-item link to="/providers" prepend-icon="mdi-rhombus-split">
                    <v-list-item-title>Πάροχοι</v-list-item-title>
                </v-list-item>
                <v-list-item link to="/contracts" prepend-icon="mdi-folder-multiple-outline">
                    <v-list-item-title>Συμβάσεις</v-list-item-title>
                </v-list-item>
            </v-list>
        </v-navigation-drawer>
        <ClientOnly>
            <NavBar @close-drawer="drawer = !drawer" />
        </ClientOnly>
        <v-main>
            <v-container>
                <slot />
            </v-container>
        </v-main>
    </v-app>
</template>
