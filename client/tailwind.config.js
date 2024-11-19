/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      fontFamily: {
        'jetbrains-mono': ['"Jetbrains Mono"', 'monospace'],
        'inter': ['"Inter"', 'sans-serif'],
      },
    },
  },
  plugins: [],
}

