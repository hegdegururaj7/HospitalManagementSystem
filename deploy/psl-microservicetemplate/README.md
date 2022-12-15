# PSL Microservice Template Helm chart

## Editable values in values.yaml

### environment (String)
The name of the environment used to load application configuration.
This will make the service load the AppSettings.\<environment>.json file.

**Default value:** Development

Note that some values specified in values.yaml are passed to the service as environment variables which will override the appSettings.json files.

### serviceAccountName (String)
The name of the service account to use.

If not set, a name is generated using the fullname helper in _helpers.tpl. This will be:
- psl-microservicetemplate if the Release name is "psl-microservicetemplate"
- psl-microservicetemplate-\<releaseName> if the Release name is anything else

Set explicitly to **default** to use the default Kubernetes service account (not recommended)

**Default value:** *empty string*

### autoscaler (Boolean)
Whether to use a HorizontalPodAutoscler. Set to true for production.
If false, a ReplicaSet with a single replica pod is created.

**Default value:** false

### apiTag (String)
The version tag for the API container image to use.
If not set, the appVersion value in Chart.yaml is used

**Default value:** *empty string*

Set the value of this field to install a specific container image. Images are labelled with the CD pipeline release number whenever a release is generated. The deployment pipeline will set this value to control the version of code being deployed, so this should only be used in manual overrides of the release process.

### containerPort (Integer)
The TCP ports to be exposed by the service container.

**Default value:** 7011

### servicePort (Integer)
The TCP ports to be exposed internally to the cluser and externally by the service.

**Default value:** 7011

### useLoadBalancer (Boolean)
Whether to use a Load Balancer controller. Set to false for production. 
If false, the service is only available within the cluster
If true, an external IP is added to allow connections from outside the Kubernetes cluster. Useful for developing against this service locally or in Integration

**Default value:** false
