export function formatName(name): string {
    const returnName = name.replace(/_/g, ' ');
    return returnName.replace(/\w\S*/g, function (str) {
        return str.charAt(0).toUpperCase() + str.substr(1).toLowerCase();
    });
}

export function decapitalizaAndRemoveSpaces(name): string {
    const returnName = name.replace(/ /g, '_');
    return returnName.toLowerCase();
}
