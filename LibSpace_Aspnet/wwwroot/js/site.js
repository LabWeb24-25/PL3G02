﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function previewImage(event, previewId) {
    const file = event.target.files[0];
    const preview = document.getElementById(previewId);
    const removeButton = document.getElementById('removeImageButton'); // Corrected ID

    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            preview.src = e.target.result;
            preview.style.display = "block"; // Show the image preview
            removeButton.style.display = "block"; // Show the remove button
        };
        reader.readAsDataURL(file);
    } else {
        preview.src = "#";
        preview.style.display = "none"; // Hide the preview
        removeButton.style.display = "none"; // Hide the remove button
    }
}

// Function to remove the image preview and reset the input
function removeImage(inputId, previewId, buttonId) {
    const fileInput = document.getElementById(inputId);
    const preview = document.getElementById(previewId);
    const removeButton = document.getElementById(buttonId);

    fileInput.value = ""; // Clear the file input
    preview.src = "#"; // Reset the preview image source
    preview.style.display = "none"; // Hide the preview
    removeButton.style.display = "none"; // Hide the remove button
}
