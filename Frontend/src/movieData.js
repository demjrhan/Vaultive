import { renderMovies } from './renderMovies.js';

const API_BASE_URL = 'http://localhost:5034/api';

export const featuredMovie = {
  publisher: 'A Disney Original Film',
  title: 'RAYA AND THE LAST DRAGON',
  imageSrc: 'public/img/star/raya-the-last-dragon-star-container.png',
};

export async function fetchMovieData() {
  try {
    const response = await fetch(`${API_BASE_URL}/Movie/All`);
    if (!response.ok) throw new Error('Failed to fetch movies');
    return await response.json();
  } catch (error) {
    console.error('Error loading movies:', error);
    return [];
  }
}

export async function searchTextListener() {
  const searchInput = document.querySelector('.search-area');
  if (!searchInput) return;

  let debounceTimer;

  searchInput.addEventListener('input', () => {
    clearTimeout(debounceTimer);

    debounceTimer = setTimeout(async () => {
      const query = searchInput.value.trim();

      if (query.length === 0) {
        const movies = await fetchMovieData();
        await renderMovies(movies);
        return;
      }

      try {
        const response = await fetch(`${API_BASE_URL}/Movie/Search?text=${query}`);
        const movies = await response.json();
        await renderMovies(movies);
      } catch (error) {
        console.error('Error loading movies:', error);
      }
    }, 250);
  });
}
