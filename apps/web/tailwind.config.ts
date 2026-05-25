import type { Config } from 'tailwindcss';

const config: Config = {
  content: [
    './app/**/*.{ts,tsx}',
    './components/**/*.{ts,tsx}',
    './lib/**/*.{ts,tsx}',
  ],
  theme: {
    extend: {
      colors: {
        pine: {
          navy: '#1B3F6B',
          'navy-mid': '#254d80',
          green: '#6ab04c',
          'green-light': '#7cc95e',
          'green-pale': '#e8f5e3',
          'green-dark': '#4a8a32',
        },
      },
      fontFamily: {
        sans: ['DM Sans', 'sans-serif'],
        mono: ['DM Mono', 'monospace'],
      },
    },
  },
  plugins: [require('@tailwindcss/forms')],
};

export default config;
