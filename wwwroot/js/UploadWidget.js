const cloudName = "dzgrquvrs";
const uploadPreset = "Default";

const myWidget = cloudinary.createUploadWidget(
    {
        cloudName: cloudName,
        uploadPreset: uploadPreset,
        cropping: true, 
        croppingAspectRatio: 1.25, 
        croppingShowDimensions: true, 
        croppingDefaultSelectionRatio: 1.25, 
        showSkipCropButton: false, 
        multiple: false,
        sources: ["local"],
        maxImageFileSize: 2000000,
        maxImageWidth: 2000,
    },
    (error, result) => {
        if (!error && result && result.event === "success") {
            console.log(result);
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

            // Create the image container with fixed aspect ratio
            const imageContainer = document.createElement('div');
            imageContainer.className = 'ratio ratio-4x3';

            // Create the image element
            const preview = document.createElement('img');
            preview.src = result.info.thumbnail_url;
            preview.className = 'card-img-top object-fit-cover';
            preview.alt = 'Preview Image';

            // Add image to container
            imageContainer.appendChild(preview);

            // Create card body for the remove button
            const cardBody = document.createElement('div');
            cardBody.className = 'card-body p-2';

            // Create remove button
            const removeButton = document.createElement('button');
            removeButton.type = 'button';
            removeButton.className = 'btn btn-sm btn-outline-danger w-100';
            removeButton.textContent = 'Remove';
            removeButton.onclick = function() {
                input.remove();
                colDiv.remove();
            };

            // Assemble the card structure
            cardDiv.appendChild(imageContainer);
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