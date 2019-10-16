package com.jetbrains.rd.framework.test.util

import com.jetbrains.rd.util.error
import com.jetbrains.rd.util.getLogger
import com.jetbrains.rd.util.info
import com.jetbrains.rd.util.reactive.IScheduler
import java.util.concurrent.LinkedBlockingDeque

object SequentialPumpingScheduler : IScheduler {
    private val q = LinkedBlockingDeque<() -> Unit>()
    override fun queue(action: () -> Unit) {
        if (isExecuting)
            q.add(action)
        else
            action()
    }


    private fun execute(action:() -> Unit) {
        val old = isActive
        isExecuting = true
        try {
            action()
        } catch (e: Throwable) {
            getLogger<SequentialPumpingScheduler>().error(e)
        } finally {
            isExecuting = old
        }
    }

    override val isActive: Boolean get() = true

    var isExecuting: Boolean = false
        private set


    override fun flush() {
        while (true) {
            val next = q.pollFirst() ?: return
            execute(next)
        }
    }

}
