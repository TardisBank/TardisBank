export const makeIdentifier = () => {
    const characters = '0123456789abcdefghijklmnopqrtsuvwxzyABCDEFGHIJKLMNOPQRTSUVWXZY';
    const idLength = 8;
    let result = '';

    for (let i = 0; i < idLength; i++) {

        let characterIndex = Math.random() * characters.length;
        result += characters.charAt(characterIndex);
    }

    return result;
}