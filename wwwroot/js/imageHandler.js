function initializeImageDeletion() {
    document.querySelectorAll('.delete-image-btn').forEach(button => {
        button.addEventListener('click', async (e) => {
            e.preventDefault();

            const imageUrl = e.currentTarget.dataset.imageUrl;
            const cardElement = e.currentTarget.closest('.col-12');

            try {
                const response = await fetch(`/FosterUpdateInfo/DeleteImage?imageUrl=${encodeURIComponent(imageUrl)}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        // Add anti-forgery token if needed
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                    }
                });

                if (response.ok) {
                    // Animate and remove the card
                    cardElement.style.transition = 'opacity 0.3s ease-out';
                    cardElement.style.opacity = '0';
                    setTimeout(() => {
                        cardElement.remove();
                    }, 300);
                } else {
                    console.error('Failed to delete image');
                    alert('Failed to delete image. Please try again.');
                }
            } catch (error) {
                console.error('Error deleting image:', error);
                alert('An error occurred while deleting the image. Please try again.');
            }
        });
    });
}

document.addEventListener('DOMContentLoaded', initializeImageDeletion);