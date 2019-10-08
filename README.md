# ODataDtoRepro

This repo contains two branches, each of which demonstrates a different approach in utilizing DTO entities in an OData server-client app.
It uses [`https://www.nuget.org/packages/Microsoft.AspNetCore.OData`](https://www.nuget.org/packages/Microsoft.AspNetCore.OData) at server-side, and [`Simple.OData.V4.Client`](https://www.nuget.org/packages/Simple.OData.V4.Client) (SOC) at the client.

Both approaches fail, and I'm looking for a decent way to get them together, or an approach in solving it in the SOC repo.

The sample also makes use of an inherited class, to demonstrate the SOC's lack of providing information about the deserialized type gathered from the response JSON.
