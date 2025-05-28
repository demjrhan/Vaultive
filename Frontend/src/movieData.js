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
    console.error('Error loading recommended movies:', error);
    return [];
  }
}
