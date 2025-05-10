import { useRouteError, isRouteErrorResponse, Link } from 'react-router';

function ErrorPage() {
  const error = useRouteError()

  if (isRouteErrorResponse(error)) {
    return (
      <div>
        <h1>
          {error.status} - {error.statusText}
        </h1>
        <p>{error.data || 'Terjadi kesalahan pada permintaan.'}</p>
        <p>
          <Link to="/">Kembali ke Beranda</Link>
        </p>
      </div>
    )
  } else if (error instanceof Error) {
    return (
      <div>
        <h1>500 - Kesalahan Internal</h1>
        <p>{error.message}</p>
        <p>
          <Link to="/">Kembali ke Beranda</Link>
        </p>
      </div>
    )
  } else {
    return (
      <div>
        <h1>Terjadi Kesalahan</h1>
        <p>Maaf, terjadi kesalahan yang tidak diketahui.</p>
        <p>
          <Link to="/">Kembali ke Beranda</Link>
        </p>
      </div>
    )
  }
}

export default ErrorPage
