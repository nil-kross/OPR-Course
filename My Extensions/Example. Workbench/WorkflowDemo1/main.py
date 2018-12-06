def onBeforeReset(task): 
	msg = getPrintMessage('pre-reset', task) 
	print msg 
def onAfterReset(task): 
	msg = getPrintMessage('post-reset', task) 
	print msg 
def onBeforeRefresh(task): 
	msg = getPrintMessage('pre-refresh', task) 
	print msg 
def onAfterRefresh(task): 
	msg = getPrintMessage('post-refresh', task) 
	print msg 
def onBeforeUpdate(task):
	if task.Name == "Engineering Data": 
		msg = getPrintMessage('post-update', task) 
		print msg 
	else: 
		print("ignored") 
def onAfterUpdate(task): 
	if task.Name == "Engineering Data": 
		msg = getPrintMessage('post-update', task) 
		print msg 
	else: 
		print("ignored") 
def onBeforeDuplicate(task): 
	msg = getPrintMessage('pre-duplicate', task) 
	print msg 
def onAfterDuplicate(task): 
	msg = getPrintMessage('post-duplicate', task) 
	print msg 
def onBeforeSourcesChanged(task): 
	msg = getPrintMessage('pre-sources-changed', task) 
	print msg 
def onAfterSourcesChanged(task): 
	msg = getPrintMessage('post-sources-changed', task) 
	print msg 
def onBeforeCreate(task): 
	msg = getPrintMessage('pre-creation', task) 
	print msg 
def onAfterCreate(task): 
	msg = getPrintMessage('post-creation', task) 
	print msg 
def onBeforeDelete(task): 
	msg = getPrintMessage('pre-deletion', task) 
	print msg 
def onAfterDelete(task): 
	msg = getPrintMessage('post-deletion', task) 
	print msg 
def onBeforeCanUseTransfer(sourceTask, targetTask): 
	msg = 'in pre-can-use-transfer with source task ' + sourceTask.Name + ' and target task ' + targetTask.Name 
	print msg 
def onAfterCanUseTransfer(sourceTask, targetTask): 
	msg = 'in post-can-use-transfer with source task ' + sourceTask.Name + ' and target task ' + targetTask.Name 
	print msg 
def onBeforeCanDuplicate(): 
	msg = getPrintMessage('pre-can-use-transfer', None) 
	print msg 
def onAfterCanDuplicate(): 
	msg = getPrintMessage('post-can-use-transfer', None) 
	print msg 
def onBeforeStatus(task): 
	msg = getPrintMessage('pre-status', task) 
	print msg 
def onAfterStatus(task): 
	msg = getPrintMessage('post-status', task) 
	print msg 
def onBeforePropertyRetrieval(task): 
	msg = getPrintMessage('pre-property', task) 
	print msg 
def onAfterPropertyRetrieval(task): 
	msg = getPrintMessage('post-property', task) 
	print msg 
def getPrintMessage(msg, task): 
	taskName = 'none' 
	if task != None: 
		taskName = task.Name 
	return 'in ' + msg + ' callback for task ' + taskName