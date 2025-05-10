import { useRouteError, isRouteErrorResponse } from 'react-router'
import NotFound from './type/not-found'
import BadGateway from './type/bad-gateway'
import RouteError from './type/route-error'
import InternalError from './type/internal-error'
import UnknownError from './type/unkwon-error'

function ErrorPage() {
  const error = useRouteError()

  if (isRouteErrorResponse(error)) {
    switch (error.status) {
      case 404:
        return <NotFound />
      case 502:
        return <BadGateway />
      default:
        return (
          <RouteError
            status={error.status}
            statusText={error.statusText}
            message={typeof error.data === 'string' ? error.data : undefined}
          />
        )
    }
  } else if (error instanceof Error) {
    return <InternalError message={error.message} />
  } else {
    return <UnknownError />
  }
}

export default ErrorPage
