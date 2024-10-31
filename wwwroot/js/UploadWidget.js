const cloudName = "dzgrquvrs";
const uploadPreset = "Default";

const myWidget = cloudinary.createUploadWidget(
    {
        cloudName: cloudName,
        uploadPreset: uploadPreset,
        cropping: true,
        sources: ["local"],
        multiple: true,
        maxImageFileSize: 2000000,
        maxImageWidth: 2000,
    },
    (error, result) => {
        if (!error && result && result.event === "success") {
            // Create a hidden input to store the URL
            const input = document.createElement('input');
            input.type = 'hidden';
            input.name = 'CloudinaryUrl';
            input.value = result.info.secure_url;
            document.querySelector('form').appendChild(input);

            // Create the preview card structure
            const colDiv = document.createElement('div');
            colDiv.className = 'col-12 col-sm-6 col-md-4 col-lg-3';

            const cardDiv = document.createElement('div');
            cardDiv.className = 'card h-100';

            // Create the image element
            const preview = document.createElement('img');
            preview.src = result.info.thumbnail_url;
            preview.className = 'card-img-top';
            preview.alt = 'Preview Image';
            preview.style.objectFit = 'cover';
            preview.style.height = '200px';

            // Create card body for the remove button
            const cardBody = document.createElement('div');
            cardBody.className = 'card-body';

            // Create remove button
            const removeButton = document.createElement('button');
            removeButton.type = 'button';
            removeButton.className = 'btn btn-sm btn-outline-danger w-100';
            removeButton.textContent = 'Remove';
            removeButton.onclick = function() {
                // Remove the hidden input as well when removing the preview
                input.remove();
                colDiv.remove();
            };

            // Assemble the card structure
            cardDiv.appendChild(preview);
            cardBody.appendChild(removeButton);
            cardDiv.appendChild(cardBody);
            colDiv.appendChild(cardDiv);

            // Insert the new preview before the upload button
            const imagePreview = document.querySelector('#imagePreview');
            const uploadButton = imagePreview.querySelector('.mb-3');
            imagePreview.insertBefore(colDiv, uploadButton);
        }
    }
);

document.getElementById("upload_widget").addEventListener(
    "click",
    function () {
        myWidget.open();
    },
    false
);