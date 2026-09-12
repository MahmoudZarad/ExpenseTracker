/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,ts}"],

  theme: {
    extend: {
      colors: {
        primary: {
          500: "#10b981",
          600: "#059669",
          700: "#047857",
        },

        app: {
          background: "#0b1120",
          surface: "#111827",
          light: "#172033",
          border: "#263449",
        },

        income: "#34d399",
        expense: "#fb7185",
        warning: "#fbbf24",
      },
    },
  },

  plugins: [],
};
