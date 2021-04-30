export default function handleApiErrors(result) {
  if (!result.ok) {
    alert(
      `API returned ${result.status} ${result.statusText}. See details in Dev Tools Console`
    );
    throw result;
  }
  return result;
}
