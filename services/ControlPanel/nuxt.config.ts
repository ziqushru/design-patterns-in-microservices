export default defineNuxtConfig({
  runtimeConfig: {
      public: {
          GATEKEEPER_URL: process.env.GATEKEEPER_URL
      }
  },

  imports: {
      autoImport: true,
  },

  devtools: {
      enabled: true,
      telemetry: false,
      timeline: {
          enabled: true
      },
  },

  modules: [
      'vuetify-nuxt-module',
      '@vueuse/nuxt',
      '@formkit/nuxt',
      '@nuxt/image',
      '@element-plus/nuxt',
      '@pinia/nuxt'
  ],

  formkit: {
      autoImport: true,
      configFile: 'formkit.config.js',
  },

  vuetify: {
      vuetifyOptions: {
          theme: {
              defaultTheme: 'dark'
          }
      }
  },

  pinia: {
      storesDirs: ['./stores/**'],
  },

  compatibilityDate: '2024-10-28'
})