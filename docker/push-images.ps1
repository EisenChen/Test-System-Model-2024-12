$dist_registry = "localhost:5000"
$images = @(
    "app/frontend:latest", 
    "app/apigateway:latest", 
    "app/service:latest", 
    "mcr.microsoft.com/mssql/server:latest"
    )

for ($i = 0; $i -lt $images.Length; $i++) {
    $image = $images[$i]

    docker image inspect $image 2>$null | Out-Null
    if ($LASTEXITCODE -eq 1) {
        if($image.StartsWith("app/")){
            $imageName = $image -split '[/:]'            
            docker compose build $imageName[1]
        }else{
            docker pull $image
        }        
    }    

    docker tag $image $dist_registry/$image
    docker push $dist_registry/$image
}