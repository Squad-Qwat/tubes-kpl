import { Outlet } from 'react-router'
import Navbar from '../components/common/navbar'
import Footer from '../components/common/footer'

function MainLayout() {
  return (
    <>    
      <Navbar />
      <main className='overflow-hidden text-foreground text-base font-mono tracking-tight'>
        <Outlet />
      </main>
      <Footer />
    </>
  )
}

export default MainLayout
