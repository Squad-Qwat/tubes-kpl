import { Outlet } from 'react-router'
import Navbar from '../components/common/navbar'
import Footer from '../components/common/footer'

function MainLayout() {
  return (
    <>    
      <Navbar />
      <main className='bg-background text-foreground text-base font-sans'>
        <Outlet />
      </main>
      <Footer />
    </>
  )
}

export default MainLayout
