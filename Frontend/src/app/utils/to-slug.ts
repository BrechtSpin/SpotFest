export function toSlug(s: string): string {
  return s
    .trim()
    .toLowerCase()
    .replace(/[^\w\s-]/g, '')   //remove special characters
    .replace(/\s+/g, '-')       //spaces into -
    .replace(/-+/g, '-');       //remove extra -
}
