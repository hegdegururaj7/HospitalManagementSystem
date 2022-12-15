{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "helpers.fullname" -}}
{{- $name := .Chart.Name  }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "helpers.labels" -}}
helm.sh/chart: {{ printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{ include "helpers.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "helpers.selectorLabels" -}}
app.kubernetes.io/name: {{ .Chart.Name }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
The name of the service account to create and use
*/}}
{{- define "helpers.serviceAccountName" -}}
{{- default (include "helpers.fullname" .) .Values.serviceAccountName }}
{{- end }}

{{/*
The name of the configmap to create and use
*/}}
{{- define "helpers.configMapName" -}}
{{- include "helpers.fullname" . }}-config 
{{- end }}

{{/*
The container registry prefix
*/}}
{{- define "helpers.registry" -}}
{{- if .Values.registry }}
{{- printf "%s/" .Values.registry }}
{{- end }}
{{- end }}
