const API_BASE_URL = 'http://localhost:5034/api';

export async function fetchReviews() {
  try {
    const response = await fetch(`${API_BASE_URL}/Review/All`);
    if (!response.ok) throw new Error('Failed to fetch reviews');
    return await response.json();
  } catch (error) {
    console.error('Error loading reviews:', error);
    return [];
  }
}