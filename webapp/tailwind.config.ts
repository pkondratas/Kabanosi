/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './app/**/*.{js,ts,jsx,tsx}',  // Adjust paths to where your files are
    './components/**/*.{js,ts,jsx,tsx}',  // Add this line if you have components
  ],
  theme: {
    extend: {
      animation: {
        'pulse-custom': 'pulse 1.5s ease-in-out infinite',
      },
      keyframes: {
        pulse: {
          '0%': { backgroundColor: '#3498db' },
          '50%': { backgroundColor: '#3498db' },
          '100%': { backgroundColor: '#3498db' },
        },
      },
    },
  },
  plugins: [
    require('@tailwindcss/line-clamp'),
  ],
}
  