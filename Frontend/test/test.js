const button = document.getElementById('submit-review');
const textarea = document.getElementById('review-textarea');

button.addEventListener('mouseover', () => {
  textarea.style.filter = 'blur(2px)';

  textarea.parentElement.appendChild(overlay);
  textarea.readOnly = true;
});


button.addEventListener('mouseout', () => {
  textarea.style.filter = 'blur(0px)';
  const existingOverlay = document.getElementById('blur-overlay');
  if (existingOverlay) existingOverlay.remove();
  textarea.readOnly = false;
})