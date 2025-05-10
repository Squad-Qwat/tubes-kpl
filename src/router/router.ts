import { createBrowserRouter } from 'react-router'
import MainLayout from '../layout/main-layout'
import Home from '../pages/home-page'
import ErrorPage from '../pages/error/error-page'

const router = createBrowserRouter([
  {
    path: '/',
    Component: MainLayout,
    ErrorBoundary: ErrorPage,
    children: [
      {
        index: true,
        ...Home,
      },

      {},
    ],
  },
])

export default router
