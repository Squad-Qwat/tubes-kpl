import { Link } from 'react-router';

export default function NotFound() {
  return (
    <div>
      <h1>404 - Halaman Tidak Ditemukan</h1>
      <p>Maaf, halaman yang Anda cari tidak ditemukan.</p>
      <p>
        <Link to="/">Kembali ke Beranda</Link>
      </p>
    </div>
  );
}
