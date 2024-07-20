export function displayFile(fileUrl, contentType) {
    const contentTypePrefix = contentType.split('/')[0];
    switch (contentTypePrefix) {
        case "image":
            return `
                <div class="w-100 d-flex border-bottom">
                    <img src="${fileUrl}" alt="image" class="" style="height:150px; width:150px" />
                </div>`;
            break;
        case "video":
            return `
                <video width="300px" height="200px" controls="controls">
                    <source src="${fileUrl}" type="${contentType}" />
                </video>`;
            break;
        case "audio":
            return `
            <audio controls class="m-1">
                <source src="${fileUrl}" type="${contentType}" />
            </audio>`;
            break;
        case "text":
            return `
            <a href="${fileUrl}" class="link-opacity-10-hover ps-1">
                ${fileUrl.split('/')[5]}
            </a>`;
            break;
        default:
            return "";
            break;
    }
}