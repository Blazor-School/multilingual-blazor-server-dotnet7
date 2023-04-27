export function addCookies(key, value)
{
    document.cookie = `${key}=${value}`;
}